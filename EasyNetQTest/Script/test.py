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
        asyncPrint("{0}/100".format(i))
        asyncPost("http://localhost:5278/Publish", { "msg": "hello{0}".format(i) })

def sub():
    asyncGet("http://localhost:5148/App");

if __name__ == "__main__":
    sub()
    # pub()
