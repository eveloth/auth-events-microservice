# AVOID STARTING AN APP USING THIS SCRIPT IN PRODCUTION
# IT WILL SET PASSWORDS AND KEYS TO STATIC DEFAULT VALUES

Copy-Item -Path ".envexample" -Destination ".env"
(Get-Content .env) -replace "(PG_PASS=)'.*'", "`$1'400aaa3a-003a-439c-8d29-f10bc7767bf1'" | Out-File .env

docker compose up -d
