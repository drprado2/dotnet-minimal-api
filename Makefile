start-local-dependencies:
	- cd eng/local_environment && docker-compose up -d

stop-local-dependencies:
	- cd eng && docker-compose down

clear-data-local-dependencies:
	- cd eng && docker-compose down -v

open-jaeger:
	- open http://localhost:16686/

open-grafana:
	- open http://localhost:3000/
 
start-krakend-gateway:
	- docker run -p 8080:8080 -v "${PWD}/eng/krakend-gateway:/etc/krakend/" devopsfaith/krakend run --config /etc/krakend/krakend.json
 
apply-flyway-structure:
	- cd eng/local_environment && docker compose run --rm flyway-structure
 
apply-flyway-data:
	- cd eng/local_environment && docker compose run --rm flyway-data
 
terraform-init:
	- rm -r eng/terraform/localhost/.terraform && rm -r eng/terraform/localhost/.terraform.lock.hcl && rm -r eng/terraform/localhost/terraform.tfstate && docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform init
 
terraform-list-providers:
	- docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform providers
 
terraform-plan:
	- docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform plan
 
terraform-apply:
	- docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform apply
 
terraform-validate:
	- docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform validate
 
terraform-fmt-files:
	- docker-compose -f eng/local_environment/docker-compose.yaml run --rm terraform fmt

deploy-krakend-plugins:
	- cd eng/krakend-gateway/plugins/client/api_key_authentication &&\
		go build -buildmode=plugin -o api-key-authentication.so api_key_authentication &&\
		mv -f api-key-authentication.so ../../.. &&\
		cd ../correlation_id &&\
		go build -buildmode=plugin -o correlation-id.so correlation_id &&\
		mv -f correlation-id.so ../../.. &&\
		cd ../../proxy/proxy_wrapper &&\
		go build -buildmode=plugin -o proxy-wrapper.so proxy_wrapper &&\
		mv -f proxy-wrapper.so ../../..




