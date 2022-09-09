import requests
import threading
import urllib
import time

def post(url, data):
    requests.post(url, data)

def get(url, data={}):
    url += "?" + urllib.parse.urlencode(data)
    requests.get(url)

def asyncPost(url, data):
    threading.Thread(target=post, args=(url, data)).start()

def asyncGet(url, data={}):
    threading.Thread(target=get, args=(url, data)).start()

def asyncPrint(*args):
    threading.Thread(target=print, args=args).start();

def pub():
    for i in range(100):
        time.sleep(0.5)
        asyncPrint("{0}/10000".format(i))
        asyncPost("http://localhost:5278/Publish", { "msg": "Publish{0}".format(i) })

def pubapp():
    for i in range(500):
        time.sleep(0.1)
        asyncPrint("{0}/500".format(i))
        asyncPost("http://localhost:5148/App/a", { "msg": "App-{0}".format(i) })
        asyncPost("http://localhost:5148/App/b", { "msg": "App-{0}".format(i) })
        asyncPost("http://localhost:5148/App/c", { "msg": "App-{0}".format(i) })
        asyncPost("http://localhost:5148/App/d", { "msg": "App-{0}".format(i) })
        asyncPost("http://localhost:5148/App/e", { "msg": "App-{0}".format(i) })

def sub():
    asyncGet("http://localhost:5148/App");

if __name__ == "__main__":
    # sub()
    # pub()
    pubapp()
