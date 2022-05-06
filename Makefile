sln_file := src/VanLaw.AdventOfCode.sln
cli_proj_file := src/VanLaw.AdventOfCode.CLI/VanLaw.AdventOfCode.CLI.csproj
current_dir = $(shell pwd)

restore:
	dotnet restore ${sln_file}

build: restore
	dotnet build --no-restore ${sln_file}

format:
	dotnet format ${sln_file}

test: build
	dotnet test --no-restore ${sln_file}

day_one: build
	dotnet run --project ${cli_proj_file} -- day-one --input-file ${current_dir}/input_files/day_one.txt

day_two: build
	dotnet run --project ${cli_proj_file} -- day-two --input-file ${current_dir}/input_files/day_two.txt

day_three: build
	dotnet run --project ${cli_proj_file} -- day-three --input-file ${current_dir}/input_files/day_three.txt
