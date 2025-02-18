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
  data = args.session.get("Link")

  for link in data:
    if(args.id):
      print(f"{link.get('id')}: ", end="")

    print(f"{link.get('name')} > {link.get('url')}")

def add(args: Namespace):
  args.session.post(
    url=f"Link",
    headers={
      "Content-Type": "application/json"
    },
    data=json.dumps({
      "url": args.url,
      "name": args.name
    })
  )

def delete(args: Namespace):
  args.session.delete(f"Link/{args.id}")

  print("Link deleted")
