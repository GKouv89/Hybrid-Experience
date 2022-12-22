# from django.shortcuts import render

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
def helloMobile(sid, data):
    print(sid)
    print(data)
    sio.emit('serverMessage', data)

@sio.event
def disconnect(sid):
    print('disconnect ', sid)