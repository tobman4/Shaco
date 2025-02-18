import json
from argparse import ArgumentParser, Namespace

def setup_parser(parser: ArgumentParser):
  action_parser = parser.add_subparsers(dest="action")

  list_parser = action_parser.add_parser("list", help="List all links")
  list_parser.set_defaults(func=list)
  
  list_parser.add_argument(
    "-i", "--id",
    help="Show ID and name of links",
    action="store_true"
  )

  add_parser = action_parser.add_parser("add", help="Add a new link")
  add_parser.set_defaults(func=add)

  add_parser.add_argument(
    "-u","--url",
    help="Url to redirect to",
    required=True
  )

  add_parser.add_argument(
    "-n", "--name"
  )

  delete_parser = action_parser.add_parser("rm", help="Delete a link")
  delete_parser.set_defaults(func=delete)

  delete_parser.add_argument(
    "id",
    help="ID of link to delete"
  )

def list(args: Namespace):
  result = args.session.get(
    url=f"{args.base}/Link"
  )

  if(not result.ok):
    raise Exception(f"Got {result.status_code} from server")

  data = result.json()

  for link in data:
    if(args.id):
      print(f"{link.get('id')}: ", end="")


    print(f"{link.get('name')} > {link.get('url')}")

def add(args: Namespace):
  result = args.session.post(
    url=f"{args.base}/Link",
    headers={
      "Content-Type": "application/json"
    },
    data=json.dumps({
      "url": args.url,
      "name": args.name
    })
  )

  if(not result.ok):
    raise Exception(f"Got {result.status_code} from server")

def delete(args: Namespace):
  result = args.session.delete(
    url=f"{args.base}/Link/{args.id}"
  )

  if(not result.ok):
    raise Exception(f"Got {result.status_code} from server")

  print("Link deleted")
