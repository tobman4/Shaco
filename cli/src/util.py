from typing import Any
from argparse import Namespace

from requests import Session

class ApiSession(Session):
  
  def __init__(self, base: str, *args, **kwargs) -> None:
    super().__init__(*args, **kwargs)
    self.base = base

  def request(self, method, url, *args, **kwargs) -> Any:
    url = f"{self.base}/{url}"
    result = super().request(method,url,*args,**kwargs)

    if(not result.ok):
      raise Exception(f"Got {result.status_code} from server");
    
    if(len(result.content) == 0):
      return None
    
    return result.json()

class Args(Namespace):
  session: Session

  def __init__(self, **kwargs: Any) -> None:
    super().__init__(**kwargs)
  
