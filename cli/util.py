from requests import Session

class ApiSession(Session):
  
  def __init__(self, base: str, *args, **kwargs) -> None:
    super().__init__(*args, **kwargs)
    self.base = base

  def request(self, method, url, *args, **kwargs):
    url = f"{self.base}/{url}"
    result = super().request(method,url,*args,**kwargs)

    if(not result.ok):
      raise Exception(f"Got {result.status_code} from server");

    return result.json()
