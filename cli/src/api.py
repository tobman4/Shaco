import json
from getpass import getpass

import requests

import ttoken

def get_auth():
  print("Username: ", end="")
  user = input()

  pw = getpass()

  return (user,pw)

def login(base: str):
  # Test saved token first
  saved = test_token(base)
  if(saved is not None):
    return saved


  auth = get_auth()

  result = requests.post(
    f"{base}/User/Login",
    headers={
      "Content-Type": "application/json"
    },
    data=json.dumps({
      "Username": auth[0],
      "Password": auth[1]
    })
  )

  if(not result.ok):
    raise Exception(f"Fail to login. Got {result.status_code}");
  
  token = result.headers["X-Token"]
  ttoken.save_token(token)

  return token

def test_token(base: str) -> str | None:
  token = ttoken.try_get_token()
  if(token is None):
    return None

  result = requests.get(
    f"{base}/Link",
    headers={
      "Authorization": f"Bearer {token}"
    }
  )

  if(not result.ok):
    return None

  return token

