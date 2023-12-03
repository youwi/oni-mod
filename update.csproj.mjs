
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

</Project>
    `
 
    FS.readdirSync(".").forEach(dir => {
        if(FS.lstatSync(dir).isDirectory()) {
            var assName=dir;
            var dirsLevelB= FS.readdirSync(dir).forEach(dirb=>{
                var filename=dir+"/"+dirb;
                var filenameNew=dir+"/"+dir+".csproj";
                if(dirb.endsWith("csproj")){
                  console.log("---"+filename)
                  FS.writeFileSync(filename,templateFull.replace("AAAAAAAAA",assName).replace("BBBBBBBBBB",assName))
                  FS.renameSync(filename,filenameNew)
                }
            });
        }
      });
}
updateAll()