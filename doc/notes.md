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
1) App Service pricing tier didn't had noticable impact (Basic and P2V2 was tested).
2) Premium storage with different quotas didn't have noticable impact.
