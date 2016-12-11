from select import select
import serial
import threading
import time
import sys
import os
import socket
from tendo import singleton
import serial.tools.list_ports


def handleBarcodeReader():
    ports = list(serial.tools.list_ports.comports())

    foundBarcodeReader = False
    for p in ports:
        if "Honeywell" in p[1]:
            ser = serial.Serial(p[0], 9600)
            foundBarcodeReader = True

    if(not foundBarcodeReader):
        os._exit(1)

    #ser = serial.Serial('COM5', 9600)
    line = b''
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    while True:
        try:
            c = ser.read()
        except:
            os._exit(1)
        if c == b'\r' or c == b'\n':
            sock.sendto('[BARCODE] %s' % line.decode('utf-8'), ("127.0.0.1", 25565))
            sys.stdout.flush()
            line = b''
        else:
            line += c

def handleRFIDReader():

    ports = list(serial.tools.list_ports.comports())
    foundBarcodeReader = False
    for p in ports:
        if "CH340" in p[1]:
            ser = serial.Serial(p[0], 9600)
            foundBarcodeReader = True

    if(foundBarcodeReader==False):
        os._exit(1)


    #ser = serial.Serial('COM7', 9600)
    line = b''
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    while True:
        try:
            c = ser.read()
        except:
            os._exit(1)
        if c == b'\r' or c == b'\n':
            sock.sendto('[RFID] %s' % line.decode('utf-8'), ("127.0.0.1", 25565))
            sys.stdout.flush()
            line = b''
        else:
            line += c


me = singleton.SingleInstance()

thread_barcode = threading.Thread(target = handleBarcodeReader)
thread_barcode.start()

thread_barcode = threading.Thread(target = handleRFIDReader)
thread_barcode.start()

