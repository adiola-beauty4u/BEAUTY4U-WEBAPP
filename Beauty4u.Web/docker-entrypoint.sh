#!/bin/sh
set -e

cat <<EOF > /usr/share/nginx/html/assets/config/config.json
{
  "apiBaseUrl": "${API_BASE_URL}"
}
EOF

exec "$@"
