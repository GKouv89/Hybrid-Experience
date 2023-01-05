from rest_framework import serializers
from .models import Player, Room

class PlayerSerializer(serializers.ModelSerializer):
    class Meta:
        model = Player
        fields = ['username', 'device']

class RoomSerializer(serializers.ModelSerializer):
    class Meta:
        model = Room
        fields = '__all__'

class RoomCreateSerializer(serializers.Serializer):
    user = serializers.CharField(max_length=30)
    name = serializers.CharField(max_length=30)

class RoomUpdateSerializer(serializers.Serializer):
    user = serializers.CharField(max_length=30)