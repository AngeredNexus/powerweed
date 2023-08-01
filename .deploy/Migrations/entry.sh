#!/bin/bash

# Проверяем, не был ли восстановлен файл-маркер при восстановлении контекста контейнера
if [ -f /tmp/ready.lock ]; then
  rm -f /tmp/ready.lock
fi

ls -la1
ls -la1 WeedDatabase
dotnet WeedDatabase.dll

echo "W8" > /tmp/ready.lock && \
sleep 10 && \
rm -f /tmp/ready.lock