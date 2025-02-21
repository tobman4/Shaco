from argparse import ArgumentParser

from util import ApiSession

import api
import link
import user

parser = ArgumentParser()
parser.set_defaults(no_auth=False)

parser.add_argument(
  "-s", "--server",
  help="Base url of server. Default https://shaco.tobman.no",
  default="https://shaco.tobman.no",
  dest="base"
)

subparser = parser.add_subparsers(dest="command")

link_parser = subparser.add_parser("link", help="Manage links")
link.setup_parser(link_parser)

user_parser = subparser.add_parser("user", help="Manage users")
user.setup_parser(user_parser)

args = parser.parse_args()

def main():
  if("func" not in args):
    print("No command")
    exit(0)
  
  args.session = ApiSession(args.base)

  if(not args.no_auth):
    auth = api.login(args.base)
    args.session.headers["Authorization"] = f"Bearer {auth}"

  args.func(args)

if __name__ == "__main__":
  main()
