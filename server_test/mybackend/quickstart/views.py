# from django.shortcuts import render
from rest_framework import generics, viewsets
from rest_framework.response import Response
from rest_framework import status
from .serializers import *
from .models import Player, Room

# Create your views here.
async_mode = None
import os
import io
from django.core.files.images import ImageFile
from django.http import HttpResponse
import socketio

basedir = os.path.dirname(os.path.realpath(__file__))
sio = socketio.Server(async_mode='eventlet')
   
@sio.event
def connect(sid, environ, auth):
    print('connect ', sid)

@sio.event
def hello(sid, data):
    print(sid)
    print(data)
    f = open('./tex.png', 'wb')
    f.write(data)
    f.close()
    sio.emit('serverMessage', 'hello from desktop')
    # This is for when there will be a model
    # image = ImageFile(io.BytesIO(data), name='tex.png')

@sio.event
def helloToRoomDesktop(sid, data):
    print(sid)
    print(data)
    sio.emit('serverMessage', data, room='chat_users0', skip_sid=desktopSid[0])

@sio.event
def helloToRoomMobile(sid, data):
    print(sid)
    print(data)
    sio.emit('serverMessage', data, room='chat_users0', skip_sid=mobileSid[0])

@sio.event
def helloMobile(sid, data):
    print(sid)
    print(data)
    sio.emit('serverMessage', data)

desktopSid = []
mobileSid = []
@sio.event
def begin_chat(sid, data):
    print(data)
    roomName = 'chat_users' + str(data['roomNo'])
    print(roomName)
    if data['type'] == "desktop":
        desktopSid.append(sid)
    else:
        mobileSid.append(sid)
    sio.enter_room(sid, roomName)

@sio.event
def disconnect(sid):
    print('disconnect ', sid)

class CreatePlayerViewSet(generics.CreateAPIView):
    queryset = Player.objects.all()
    lookup_field = 'username'
    serializer_class = PlayerSerializer

class DestroyPlayerViewSet(generics.DestroyAPIView):
    queryset = Player.objects.all()
    lookup_field = 'username'
    serializer_class = PlayerSerializer

class RoomViewSet(viewsets.ModelViewSet):
    queryset = Room.objects.all()
    lookup_field = 'name'
    serializer_class = RoomSerializer

    action_serializers = {
        'create': RoomCreateSerializer,
        'partial_update': RoomUpdateSerializer
    }

    def get_serializer_class(self):
        if self.action in self.action_serializers.keys():
            return self.action_serializers[self.action]
        # Default case
        return super().get_serializer_class()

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer_class()(data=request.data)
        if serializer.is_valid():
            user = Player.objects.get(username=serializer.validated_data['user'])
            new_room = Room.objects.create(name=serializer.validated_data['name'])
            new_room.save()
            user.room = new_room
            user.is_room_owner = True
            user.save()
            return Response(serializer.validated_data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    
    def partial_update(self, request, *args, **kwargs):
        serializer = self.get_serializer_class()(data=request.data)
        if serializer.is_valid():
            user = Player.objects.get(username=serializer.validated_data['user'])
            room = self.get_object()
            if user in room.player_set.all():
                print('here')
                # User is leaving room
                user.room = None
                user.is_room_owner = False # In case he owned the room
                user.save()
                if not room.player_set.all():
                    room.delete()
                return Response(status=status.HTTP_200_OK)
            else:
                # User is entering room
                user.room = room
                user.save()
                return Response(serializer.validated_data, status=status.HTTP_206_PARTIAL_CONTENT)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    