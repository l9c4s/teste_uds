#!/bin/bash
echo "📋 Logs do sistema:"

if [ -z "$1" ]; then
    docker-compose logs -f
else
    docker-compose logs -f $1
fi