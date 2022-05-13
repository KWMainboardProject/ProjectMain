from class_file import *
import json

def testMainCategoryContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath + ip + "maincategory.json"
    path = JsonPath(filepath)

    container = MainCategoryContainer(str(ip))  # 알아서 초기화 하세요
    container.setBox([12, 54, 32, 99])
    container.setLabel(["Top"])  # 상의
    cdict = {'lable': ["Top"], 'box': [12,54,32,99]}
    container.setDict(cdict)

    WriteJsonFile(container.getDict(), path.getPath())

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())
    return path

def testSubCategoryContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath + ip + "subcategory.json"
    path = JsonPath(filepath)

    container = SubCategoryContainer(str(ip))  # 알아서 초기화 하세요
    container.setLabel(["neat"])
    cdict = {'lable': ["neat"]}
    container.setDict(cdict)

    WriteJsonFile(container.getDict(), path.getPath())

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())
    return path

def testPatternContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath + ip + "/pattern.json"
    path = JsonPath(filepath)

    container = PatternContainer(str(ip))  # 알아서 초기화 하세요
    container.setLabel(["solid"])
    cdict = {'lable': ["solid"]}
    container.setDict(cdict)

    WriteJsonFile(container.getDict(), path.getPath())

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())
    return path

def testStyleContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath + ip + "/style.json"
    path = JsonPath(filepath)

    container = StyleContainer(str(ip))  # 알아서 초기화 하세요
    container.setLabel(["casual"])
    cdict = {'lable': ["casual"]}
    container.setDict(cdict)

    WriteJsonFile(container.getDict(), path.getPath())

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())
    return path


def WriteJsonFile(dict, json_path):
    with open(json_path, 'w') as f:
        json.dump(dict, f)


def ReadJsonFile(json_path):
    with open(json_path, 'r') as f:
        json_data = json.load(f)
    return json_data


if __name__ == "__main__":
    id = "Sample"   # user_ip or process id... 등 구별 가능한걸로 설정
    fashion = FashionContainer(id) #생성자는 원하는대로 넣으세요
    path = JsonPath("./" + id + ".json")

    mc_path = testMainCategoryContainer(id, "main img")
    sc_path = testSubCategoryContainer(id, "crop img")
    p_path = testPatternContainer(id, "crop img")
    s_path = testStyleContainer(id, "main img")

    fashion.mc.setDict(ReadJsonFile(mc_path).getPath())
    fashion.sc.setDict(ReadJsonFile(sc_path).getPath())
    fashion.pc.setDict(ReadJsonFile(p_path).getPath())
    fashion.st.setDict(ReadJsonFile(s_path).getPath())

    WriteJsonFile(fashion.getDict(), path.getPath())
    print(fashion.getDict())
