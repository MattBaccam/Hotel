ECHO off 
sqlcmd -S localhost -E -i hoteldb.sql

rem server is a local host

ECHO . 
ECHO if no errors appear Db was created
PAUSE