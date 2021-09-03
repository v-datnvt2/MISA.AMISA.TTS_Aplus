**Title**: **MISA AMIS PROJECT**\
**Author**: __[Nguyen Viet Thanh Dat](https://https://github.com/v-datnvt2)__\
**Date**: *2021-09-03*

# MISA AMIS PROJECT

## Requirements
- __[Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)__
- __[dbForge Studio](https://www.devart.com/dbforge/mysql/studio/download.html)__
- __[MariaDB](https://mariadb.org/)__
- __[NodeJS](https://nodejs.org/en/download/)__
- __[VueJS](https://vuejs.org/v2/guide/installation.html)__

## Create Databases
1. Use Command Line: Open Mysql Client and login to MariaDB.
```shell
CREATE DATABASE testdb;
USE testdb;
SOURCE /PATH/TO/misa_amis_tts_a.sql;
```
2. Use dbForge: Open dbForge and create new connection to MariaDB.
- Create new database: testdb with Charset utf8mb4 and Collation utf8mb4_unicode_ci.
- Open file misa_amis_tts_a.sql.
- F5 to execute SQL file.

## Front-End
- Disable auditing package dependencies for security vulnerabilities.
```shell
npm set audit false
```
- Setup
```shell
npm install
```
- Compiles and hot-reloads for development
```shell
npm run serve
```
## Back-End
- Open Visual Studio 2019 and open project with MISA_TTS_A.sln.
- Change config in appsettings.json: Host, username and password in MISA.AMIS.API.
- Change IIS Express to MISA.AMIS.TTS_A.
- Run project then Swagger auto launch Browser (Check API).
- Disable auto launchBrowser Swagger: Change "launchBrowser": true to "launchBrowser": false in MISA.AMIS.API/Properties/lauchSettings.json.
