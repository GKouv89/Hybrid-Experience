"""
WSGI config for mybackend project.

It exposes the WSGI callable as a module-level variable named ``application``.

For more information on this file, see
https://docs.djangoproject.com/en/4.1/howto/deployment/wsgi/
"""

import os
import eventlet
import eventlet.wsgi
import socketio

from django.core.wsgi import get_wsgi_application
from quickstart.views import sio

os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'mybackend.settings')

application = get_wsgi_application()

application = socketio.WSGIApp(sio, application)
eventlet.wsgi.server(eventlet.listen(('', 8000)), application)