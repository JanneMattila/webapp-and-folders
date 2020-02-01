# Notes

Perf testing is always tricky. Here are some
numbers with *very* **very** limited testing.
Your mileage *will* vary.

Tests with App Service when generating 2430 files:

| Test   | Create   | Delete   |
|---|---|---|
| Locally running app | 5 seconds | < 1 second |
| Locally running in container | 27 seconds | < 1 second |
| `/home` folder when `WEBSITES_ENABLE_APP_SERVICE_STORAGE=false` | 16 seconds | 2 seconds |
| `/home` folder when `WEBSITES_ENABLE_APP_SERVICE_STORAGE=true` | ~65 seconds  | 13 seconds  |
| `Standard` storage as `Mount storage (Preview)` | ~3 minutes 40 seconds | ~1 minutes 20 seconds  |
| `Premium` storage with quota `100 GiB` as `Mount storage (Preview)` | ~2 minutes | ~1 minutes 20 seconds  |
| `Premium` storage with quota `1000 GiB` as `Mount storage (Preview)` | ~2 minutes | ~1 minutes 20 seconds  |

Comments:
1) App Service pricing tier didn't had noticeable impact (Basic and P2V2 was tested).
2) Premium storage with different quotas didn't have noticeable impact.

# How to test

Deploy `jannemattila/webapp-fs-tester` from Docker Hub to your App 
Service (or create your own image using source from this repo).
Add different type of storages using `Mount storage (Preview)`.

Use Visual Studio Code with [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) extension
with this example test file [api.http](../api.http).

Here are some explanations how to test it:

1. Set target `endpoint`:

```http
@endpoint = http://localhost:32771
```

2. Generate files
* `path`: Root folder used for generating the files.
* `folders`: Number of folders to create to each directory.
* `subFolders`: Number sub-folders to create (e.g. 2 => `path1/path2`).
* `filesPerFolder`: Number of files to generate to each the leaf folders.
* `fileSize`: Generated file size in bytes.

```http
### Generate files
POST {{endpoint}}/api/generate HTTP/1.1
Content-Type: application/json

{
    "path": "/home/a",
    "folders": 3,
    "subFolders": 5,
    "filesPerFolder": 10,
    "fileSize": 1024
}
```

Above would **generate 2430 files** under the path `/home/a`.

To clean up that folder you can use following API call:

```http
### Delete files
DELETE {{endpoint}}/api/files HTTP/1.1
Content-Type: application/json

{
    "path": "/home/a",
    "recursive": true
}
```

If you need to check the files and folders under certain path
you can use following API call:

```http
### Get files
POST {{endpoint}}/api/files HTTP/1.1
Content-Type: application/json

{
    "path": "/home",
    "recursive": true,
    "filter": "*"
}
```
