#!/usr/bin/env python3
from select import select
import serial
import threading
import time
import sys
import socket


def handleBarcodeReader():

    ser = serial.Serial('COM5', 9600)
    line = b''
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    while True:
        c = ser.read()
        if c == b'\r' or c == b'\n':
            sock.sendto('[BARCODE] %s' % line.decode('utf-8'), ("127.0.0.1", 25565))
            print('[BARCODE] %s' % line.decode('utf-8'))
            sys.stdout.flush()
            line = b''
        else:
            line += c

def handleRFIDReader():

    ser = serial.Serial('COM6', 9600)
    line = b''
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    while True:
        c = ser.read()
        if c == b'\r' or c == b'\n':
            sock.sendto('[RFID] %s' % line.decode('utf-8'), ("127.0.0.1", 25565))
            print('[RFID] %s' % line.decode('utf-8'))
            sys.stdout.flush()
            line = b''
        else:
            line += c

thread_barcode = threading.Thread(target = handleBarcodeReader)
thread_barcode.start()

thread_barcode = threading.Thread(target = handleRFIDReader)
thread_barcode.start()

