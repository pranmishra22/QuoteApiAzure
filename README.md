# QuoteApiAzure
![ci](https://github.com/pranmishra22/QuoteApiAzure/actions/workflows/ci.yml/badge.svg)
![deploy](https://github.com/pranmishra22/QuoteApiAzure/actions/workflows/deploy.yaml/badge.svg)

## CURL command with responses
<img width="3494" height="1322" alt="curl_quotes" src="https://github.com/user-attachments/assets/41f41a07-cf6e-4c52-a0da-c7a5b29e2dd5" />

## GET and POST using Azure SQL
<img width="3554" height="1436" alt="db_quotes" src="https://github.com/user-attachments/assets/1f7791fe-ff24-4ee8-bee5-fbd35a031b1c" />

**Symptom:** GET /debug/slow took ~5s while other endpoints stayed in the low milliseconds.

**How I found it:**
1. Performance blade: sorted operations by duration and spotted the outlier.
2. End-to-end transaction view: ~all of the time was one SQL dependency.
3. KQL on the dependencies table confirmed the long call.
4. Used the X-Correlation-Id to find the operation_Id and followed the request through logs.
<img width="1648" height="363" alt="Slow_api_kql" src="https://github.com/user-attachments/assets/b7134304-ce60-4cb2-bc20-5115fa2da5e2" />
<img width="1707" height="912" alt="Performance_end_to_end_slow_query" src="https://github.com/user-attachments/assets/d8bbf365-cb19-44b3-86bb-d5747d870399" />

**Root cause:** a slow database call (simulated with WAITFOR DELAY).
