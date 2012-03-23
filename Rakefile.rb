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

#Add the folders that should be cleaned as part of the clean task
CLEAN = Rake::FileList.new; # how to remove the **Core rule from the default CLEAN list?
CLEAN.include(FileList["Source/**/#{CONFIGURATION}"])

CLOBBER.include(FileList["Packages/**"])

desc "Compiles solution and runs unit tests"
task :default => [:clean, :compile, :test]

desc "Executes all MSpec and NUnit tests"
task :test => [:mspec, :nunit]

desc "Compile solution file"
msbuild :compile do |msb|
    msb.properties :configuration => CONFIGURATION
    msb.targets :Clean, :Build
    msb.solution = SOLUTION_FILE
end

desc "Executes MSpec tests"
mspec :mspec  do |mspec| # => [:compile]
    specs = FileList["Source/**/#{CONFIGURATION}/*Tests.dll"].exclude(/obj\//)

    mspec.command = "Packages/Machine.Specifications.0.5.5.0/tools/mspec-x86-clr4.exe"
    mspec.assemblies = specs
end

desc "Executes NUnit tests"
nunit :nunit => [:compile] do |nunit| 
    tests = FileList["Source/**/#{CONFIGURATION}/*Tests.dll"].exclude(/obj\//)

    nunit.command = "Packages/NUnit.Runners.2.6.0.12051/tools/nunit-console.exe"
    nunit.assemblies = tests
end 
