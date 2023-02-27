from django.db import models
from django.core.validators import MaxValueValidator, MinValueValidator

class Room(models.Model):
    WAITING='WA'
    PLAYING='PL'
    ENDED='EN'
    GAME_STATE_CHOICES=[
        (WAITING, 'Waiting'),
        (PLAYING, 'Playing'),
        (ENDED, 'Ended'),
    ]
    name = models.CharField(max_length=30, unique=True)
    game_state = models.CharField(max_length=2, choices=GAME_STATE_CHOICES, default=WAITING,)



# Create your models here.
class Player(models.Model):
    DESKTOP = 'DE'
    MOBILE = 'MO'
    # HEADSET = 'VR' # Future Usage
    DEVICE_CHOICES = [
        (DESKTOP, 'Desktop'),
        (MOBILE, 'Mobile'),
    ]
    
    username = models.CharField(max_length=30, unique=True,)
    device = models.CharField(max_length=2, choices=DEVICE_CHOICES,)
    room = models.ForeignKey(Room, on_delete=models.SET_NULL, null=True, default=None, ) # At first null, until player can create or jump in a room
    is_room_owner = models.BooleanField(default=False,)
    # Maybe a field to indicate readiness to play