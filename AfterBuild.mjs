
//自动更新版本号 的脚本
import { debug } from 'console';
import FS from 'fs';
 
let replaceFileSync=function( filePath, sourceRegx, targetStr){
    var fillText= FS.readFileSync(filePath).toString();
    fillText = fillText.replace(sourceRegx,targetStr);
    FS.writeFileSync(filePath, fillText);
}

function dayVersion() {
    const date = new Date();
    const year = date.getFullYear(); // 年份，例如 2023
    const month = date.getMonth()+1; // 月份，0-11，0 表示一月，11 表示十二月
    const day = date.getDate(); // 日期，1-31
    return "version: "+year + "." + month + "." + day
}
var args = process.argv.slice(2);
var sourceDir="resource";
var projectName=args[1];
var projectDll = args[0];
var projectPdb = projectDll.substring(0, projectDll.length - 4)+".pdb"
var debugFlag = args[2];

var modConfigName=process.env.ONI_MOD_LOCAL+"/../mods.json";

if (FS.existsSync("resource/mod_info.yaml")) {
    sourceDir="resource"
} else {
    sourceDir="../../resource"
}

replaceFileSync(sourceDir+"/mod_info.yaml", /version.*/, dayVersion())
console.log(projectName + "-----Version Update 版本号已经修改" + dayVersion());

//复制resource(创建了文件夹)
FS.cpSync(sourceDir, process.env.ONI_MOD_LOCAL+"/"+projectName,{recursive: true})
console.log(projectName+"-----Resource Files 文件已经复制");

//复制DLL
try {
    var targetDll = process.env.ONI_MOD_LOCAL+"/" + projectName + "/" + projectName + ".dll";
    var targetPdb = process.env.ONI_MOD_LOCAL + "/" + projectName + "/" + projectName + ".pdb";
  
    FS.copyFileSync(projectDll, targetDll)
    console.log(projectName + "-----Mod DLL,文件已经复制:" + debugFlag);
    if (debugFlag == "Debug") {
        if (FS.existsSync(projectPdb)) {
            FS.copyFileSync(projectPdb, targetPdb)
        } else {
            FS.unlinkSync(targetPdb);
        }
        console.log(projectName + "-----Mod Pdb 文件已经复制");
    } else {
        if (FS.existsSync(targetPdb))
            FS.unlinkSync(targetPdb);
    }
    
 
} catch (e) {
    console.log(projectName + "-----Error dll copy fail, Oni Running???---");
    console.log("---"+e)
    //判断单元测试时要忽略.
    if (!projectName.includes("Test")) {
       // process.exitCode = 1;
    }
}

//没有实际效果,所以注释了
const ModConfig = JSON.parse(FS.readFileSync(modConfigName).toString());
var foundjson = false;
var expObj = {
    "label": {
        "distribution_platform": 0,
        "id": projectName,
        "title": projectName,
        "version": -550861533
    },
    "status": 1,
    "enabled": true,
    "enabledForDlc": [
        "EXPANSION1_ID"
    ],
    "crash_count": 0,
    "reinstall_path": null,
    "staticID": "Yu." + projectName
}
ModConfig.mods.forEach(element => {
   
    if (element.label.id == projectName) {
        foundjson = true;
        element.enabled = true;
        element.crash_count = 0;
    }
});
if (foundjson) {
    console.log(projectName + "-----Mod enable 模组:" + foundjson);
    FS.writeFileSync(modConfigName, JSON.stringify(ModConfig, null, 2))
} else {
    ModConfig.mods.splice(0,null,expObj);
    FS.writeFileSync(modConfigName, JSON.stringify(ModConfig, null, 2))
}

var bootConfig = "G:\\Steam\\steamapps\\common\\OxygenNotIncluded\\OxygenNotIncluded_Data\\boot.config";
replaceFileSync(bootConfig, /wait-for-native-debugger.*/, "wait-for-native-debugger=0\nplayer-connection-debug=1")
replaceFileSync(bootConfig, /player-connection-debug=1\nplayer-connection-debug.*/, "player-connection-debug=1")

console.log(projectName + "-----boot.config 已经修改" );

//wait-for-native-debugger=1
//hdr-display-enabled=0
//single-instance=1
//player-connection-debug=1
//console.log(projectName+"-----Mod enabled B 模组启用了\n");


// 有3个办法.我直接使用读文件
//https://www.stefanjudis.com/snippets/how-to-import-json-files-in-es-modules-node-js/
//const { default: info } = await import("file://"+process.env.ONI_MOD_LOCAL+"/../mods.json", { assert: { type: "json",},});
//const modConfig=require()
// import mod form ""
 