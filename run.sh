#!/bin/sh
#
# AVOID STARTING AN APP USING THIS SCRIPT IN PRODCUTION
# IT WILL SET PASSWORDS AND KEYS TO STATIC DEFAULT VALUES

cp -v .envexample .env
sed -i "/PG_PASS/s|''|'400aaa3a-003a-439c-8d29-f10bc7767bf1'|" .env &&

docker compose up -d --build
