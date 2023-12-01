
//自动更新版本号 的脚本
import FS from 'fs';

process.argv
let replaceFile = function (filePath, sourceRegx, targetStr) {

    FS.readFile(filePath,function(err,data){
        if (err) {
            console.log(err);
            return;
        }
        let str = data.toString();
        str = str.replace(sourceRegx,targetStr);
        FS.writeFile(filePath, str, function (err) {
            console.log(err);
            return;
        });
    });
}
let replaceFileSync=function( filePath, sourceRegx, targetStr){
    var fillText= FS.readFileSync(filePath).toString();
    fillText = fillText.replace(sourceRegx,targetStr);
    FS.writeFileSync(filePath, fillText);
}

function dayVersion() {
    const date = new Date();
    const year = date.getFullYear(); // 年份，例如 2023
    const month = date.getMonth(); // 月份，0-11，0 表示一月，11 表示十二月
    const day = date.getDate(); // 日期，1-31
    return "version: "+year + "." + month + "." + day
}
var args = process.argv.slice(2);
var sourceDir="resource";
var projectName=args[1];
var projectDll=args[0];
var modConfigName=process.env.ONI_MOD_LOCAL+"/../mods.json";

if (FS.existsSync("resource/mod_info.yaml")) {
    sourceDir="resource"
} else {
    sourceDir="../../resource"
}

replaceFileSync(sourceDir+"/mod_info.yaml", /version.*/, dayVersion())
console.log(projectName+"-----Version Update 版本号已经修改\n: ");

//复制resource(创建了文件夹)
FS.cpSync(sourceDir, process.env.ONI_MOD_LOCAL+"/"+projectName,{recursive: true})
console.log(projectName+"-----Resource Files 文件已经复制");

//复制DLL
try{
    FS.copyFileSync(projectDll,process.env.ONI_MOD_LOCAL+"/"+projectName+"/"+projectName+".dll")
    console.log(projectName+"-----Mod DLL 文件已经复制");
}catch(e){
    console.log(projectName+"-----Error dll copy fail, Oni Running???---");
    process.exitCode=1;
}

/*
const ModConfig=JSON.parse(FS.readFileSync(modConfigName).toString());
ModConfig.mods.forEach(element => {
    if(element.label.id==projectName){
        element.enabled=true;
        console.log(projectName+"-----Mod enabled A 模组启用了\n");
    }
});
FS.writeFileSync(modConfigName, JSON.stringify(ModConfig,null,2))
//console.log(projectName+"-----Mod enabled B 模组启用了\n");
*/

// 有3个办法.我直接使用读文件
//https://www.stefanjudis.com/snippets/how-to-import-json-files-in-es-modules-node-js/
//const { default: info } = await import("file://"+process.env.ONI_MOD_LOCAL+"/../mods.json", { assert: { type: "json",},});
//const modConfig=require()
// import mod form ""
 