import os
import pathlib

TOKEN_FILE=os.path.join(pathlib.Path.home(), ".shaco")

def try_get_token():
  if(not os.path.exists(TOKEN_FILE)):
    return None

  with(open(TOKEN_FILE, "r") as f):
    token = f.readline()
    return token

def save_token(token: str):
  with(open(TOKEN_FILE, "w+") as f):
    f.write(token)
