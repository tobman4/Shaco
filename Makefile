start_dev:
	dotnet run --environment Development --project ./src/Shaco.Api

build:
	@docker build -t nami.tobman.no:5000/shaco:1.0 .
	@docker push nami.tobman.no:5000/shaco:1.0
	
build_app:
	@dotnet pack src/Shaco.cli/ -o bin

install_app: build_app uninstall_app
	@dotnet tool install -g --add-source bin/ Shaco.cli

uninstall_app:
	@dotnet tool uninstall -g Shaco.cli
