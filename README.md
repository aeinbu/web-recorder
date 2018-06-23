# Web Recorder or Http Recorder
A webserver that records any incoming requests.

This utility helps examine incoming payloads. This can be usefull when setting up a webhooks receiver.

This utility could also act as a replayer proxy to use a web resource is offline.

Web Recorder starts up two different http endpoints, one for recording all requests, and one for examining the recordings, as well as administering the recorder.

### v1
- http endpoint(s)
  - records request path, querystring, verbs, headers, post/payload etc
  - can respond with predefined messages
- WebUI to list and examine at recordings

### v2
- proxy
  - records requests and their responses
  - authentication pass-through
- replayer
  - can replay responses to requests differing on different paths, querystring, post messages etc
  - some customization of how to select a response to a requests (what to ignore etc)

### v3
- WebUI response and request editor
