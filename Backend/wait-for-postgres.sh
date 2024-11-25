#!/bin/sh

host="$1"
port="${2:-5432}"
shift 2
cmd="$@"

until nc -z "$host" "$port"; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 20
done
sleep 20

>&2 echo "Postgres is up - executing command"
exec $cmd
