# checkZVsem
check DATEV Zahlungsverkehr ToCtrl.sem 
 
checkZVsem prüft das Datum des letzten Zugriffs der ToCtrl.sem 
 
Als Parameter wird ein Integer (Stunden) mitgegeben. 
 
## Aufruf 
checkZVsem 24

Wenn die ToCtrl.sem Datei im Verzeichnis \\<servername>\WINDVSW1\DATEV\DATEN\ZVKW\BESTAND\STANDARD\
älter als 24 Stunden ist wirk folgendes erzeugt:
 
1) Exitcode der checkZVsem.exe enthält das alter in Stunden 
2) In der Console wird die Meldung **ERROR: last access to old** ausgegeben 
3) Im Eventlog (Application, EVENTID 2111) wird ein Error erzeugt. 

