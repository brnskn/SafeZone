# coding=utf-8
import argparse
import zipfile
import os
import pycdlib
import shutil

ARCHIVE_NAME = 'safezone'
ZIP_EXT = '.safe'

def repack_iso():
    if (os.path.exists(ARCHIVE_NAME + ZIP_EXT)):
        with zipfile.ZipFile(ARCHIVE_NAME + ZIP_EXT, mode='r') as zf:
            zf.extractall(ARCHIVE_NAME)
        unmount()
        iso = pycdlib.PyCdlib()
        iso.new()
        for file in os.listdir(ARCHIVE_NAME):
            f = ARCHIVE_NAME + '/' + file
            filename, file_extension = os.path.splitext(file)
            iso.add_file(f, iso_path='/' + (filename[:8]+file_extension[:4]).upper().replace(' ', ''))
        iso.write(ARCHIVE_NAME + '.iso')
        iso.close()
        shutil.rmtree(ARCHIVE_NAME)
        print("Başarılı")
        mount()
    else:
        print("Önce zip dosyasını oluşturun.")

def add_file_to_zip(filepath):
    with zipfile.ZipFile(ARCHIVE_NAME+ZIP_EXT, mode='w') as zf:
        zf.write(filepath, arcname=os.path.basename(filepath))
    print("Başarılı")

def mount():
    if (os.path.exists(ARCHIVE_NAME + '.iso')):
        f = ARCHIVE_NAME+'.iso'
        os.system('PowerShell Mount-DiskImage -ImagePath "'+os.path.realpath(f)+'"')

def unmount():
    if (os.path.exists(ARCHIVE_NAME + '.iso')):
        f = ARCHIVE_NAME+'.iso'
        os.system('PowerShell Dismount-DiskImage -ImagePath "'+os.path.realpath(f)+'"')

parser = argparse.ArgumentParser()
parser.add_argument('-a', '--add', type=str, nargs=1)
parser.add_argument('-i', '--iso', type=bool)
parser.add_argument('-m', '--mount', type=bool)
parser.add_argument('-u', '--unmount', type=bool)
args = parser.parse_args()

if args.add:
    FILE_PATH = args.add[0]
    add_file_to_zip(filepath=FILE_PATH)
    if args.iso:
        repack_iso()
elif args.iso:
    repack_iso()
elif args.mount:
    mount()
elif args.unmount:
    unmount()