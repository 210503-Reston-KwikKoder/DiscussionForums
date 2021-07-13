.PHONY: build run clean test clearlogs publish

#solution_root is the directory containing the root .sln file
solution_root = DiscussionForum


#solution_main is the directory containing the project with a main method. Relative to solution_root.
solution_main = DiscussionForumREST

solution_dl = $(solution_root_BL)/DFDL

#log_dir is the name of directory with logs relative to solution_root
log_dir = logs
restore:
	dotnet restore ./$(solution_root);
build:
	dotnet build $(solution_root)
run: 
	dotnet run --project $(solution_root)/$(solution_main)
test:
	dotnet test $(solution_root)
publish:
	dotnet publish  $(solution_root)/$(solution_main) -c Release -o $(solution_root)/$(solution_main)/publish
rebuild-db:
    #dotnet ef database drop -c BELBDBContext --startup-project ../$(solution_root)
	cd ./$(solution_dl) && dotnet ef migrations remove --startup-project ../$(solution_main_BL);
	cd ./$(solution_dl) && dotnet ef migrations add newMigration -c BELBDBContext --startup-project ../$(solution_main_BL);
	cd ./$(solution_dl) && dotnet ef database update newMigration --startup-project ../$(solution_main_BL)
clean: clearlogs
	dotnet clean $(solution_root)
clearlogs:
	rm -f $(solution_root)/$(log_dir)/*
	
