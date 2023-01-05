from django.urls import path, include
from . import views
from rest_framework import routers

router = routers.DefaultRouter()
router.register(r'rooms', views.RoomViewSet, basename='room')

urlpatterns = [
    path('players/', views.CreatePlayerViewSet.as_view()),
    path('players/<str:username>/', views.DestroyPlayerViewSet.as_view()),
]

urlpatterns += router.urls
