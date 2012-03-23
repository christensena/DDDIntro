# encoding: utf-8
require 'rubygems'
require 'albacore'
require 'rake/clean'
require 'rexml/document'

CONFIGURATION = 'Release'
SOLUTION_FILE = 'Source/DDDIntro.sln'

Albacore.configure do |config|
    config.log_level = :verbose
    config.msbuild.use :net4
end

desc "Compiles solution and runs unit tests"
task :default => [:clean, :compile, :test]

desc "Executes all MSpec and NUnit tests"
task :test => [:mspec, :nunit]

#Add the folders that should be cleaned as part of the clean task
CLEAN = FileList["Source/**/#{CONFIGURATION}"]

desc "Compile solution file"
msbuild :compile do |msb|
    msb.properties :configuration => CONFIGURATION
    msb.targets :Clean, :Build
    msb.solution = SOLUTION_FILE
end

desc "Executes MSpec tests"
mspec :mspec => [:compile] do |mspec|
    #This is a bit fragile but this is the only mspec assembly at present. 
    #Fails if passed a FileList of all tests. Need to investigate.
    mspec.command = "Packages/Machine.Specifications.0.5.5.0/tools/mspec-x86-clr4.exe"
    mspec.assemblies "Source/UnitTests/bin/Release/DDDIntro.UnitTests.dll"
end

desc "Executes NUnit tests"
nunit :nunit => [:compile] do |nunit| 
    tests = FileList["Source/**/#{CONFIGURATION}/*Tests.dll"].exclude(/obj\//)

    nunit.command = "Packages/NUnit.Runners.2.6.0.12051/tools/nunit-console.exe"
    nunit.assemblies = tests
end 
