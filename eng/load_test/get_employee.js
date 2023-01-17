import http from 'k6/http';
import { check, sleep } from 'k6';

export default function () {
    const res = http.get('http://localhost:5067/v1/employees/5a9896e2-7889-4505-84e7-3bcf76c66b0b');
    check(res, { 'status was 200': (r) => r.status == 200 });
    sleep(1);
}