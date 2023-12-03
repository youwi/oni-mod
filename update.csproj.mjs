
import FS from "fs"
 

function updateAll(){

    var templateFull=
    `
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
	<ModTitle>AAAAAAAAA</ModTitle>
	<Description>BBBBBBBBBB</Description>
    <BaseOutputPath></BaseOutputPath>
    <AssemblyVersion>2023.12.3.30</AssemblyVersion>
    <FileVersion>1.0.0.29</FileVersion>
  </PropertyGroup>
	<Target Name="CopyAllFileToDir" AfterTargets="PostBuildEvent">
		<Exec StdErrEncoding="utf-8" StdOutEncoding="utf-8" Command="node ../AfterBuild.mjs $(TargetPath)  $(ProjectName)" />
	</Target>
</Project>
    `
 
    FS.readdirSync(".").forEach(dir => {
        if(FS.lstatSync(dir).isDirectory()) {
            var filename=dir+"/"+dir+".csproj";
            if(FS.existsSync(filename)){
                //var strOrigal=FS.readFileSync(filename);
                FS.writeFileSync(filename,templateFull.replace("AAAAAAAAA",dir).replace("BBBBBBBBBB",dir))
            }
        }
      });
}
updateAll()