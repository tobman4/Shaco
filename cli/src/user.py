import json
from getpass import getpass
from argparse import ArgumentParser, Namespace

import requests

def setup_parser(parser: ArgumentParser):
  action_parser = parser.add_subparsers(dest="action")

  add_parser = action_parser.add_parser("add")
  add_parser.set_defaults(func=add_user)
  add_parser.set_defaults(no_auth=False)

  list_parser = action_parser.add_parser("list")
  list_parser.set_defaults(func=list)

def add_user(args: Namespace):
  print("Username: ", end="")
  username = input()
  passowrd = getpass()

  result = requests.post(
    f"{args.base}/User",
    headers={
      "Content-Type": "application/json",
    },
    data=json.dumps({
      "Username": username,
      "Password": passowrd
    })
  )

  if(not result.ok):
    raise Exception(f"Got {result.status_code} from server")
  
  print(f"Added user {username}")

def list(args: Namespace):
  users = args.session.get("User")

  print(users)
