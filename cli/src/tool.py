from argparse import ArgumentParser

from util import ApiSession

import api
import link

parser = ArgumentParser()
parser.set_defaults(base="http://127.0.0.1:5148")

subparser = parser.add_subparsers(dest="command")

link_parser = subparser.add_parser("link", help="Manage links")
link.setup_parser(link_parser)

args = parser.parse_args()

def main():
  if("func" not in args):
    print("No command")
    exit(0)
  
  auth = api.login(args.base)
  args.session = ApiSession(args.base)
  args.session.headers["Authorization"] = f"Bearer {auth}"

  args.func(args)

if __name__ == "__main__":
  main()
