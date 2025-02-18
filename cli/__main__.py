from argparse import ArgumentParser

from requests import Session

import link

parser = ArgumentParser()
parser.set_defaults(base="http://127.0.0.1:5148")

subparser = parser.add_subparsers(dest="command")

link_parser = subparser.add_parser("link", help="Manage links")
link.setup_parser(link_parser)

args = parser.parse_args()

if __name__ == "__main__":
  if("func" not in args):
    print("No command")
    exit(0)

  args.session = Session()

  args.func(args)
