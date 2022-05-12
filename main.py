from class_file import *
import json

def testMainCategoryContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath+ip+"maincategory.json"
    path = JsonPath(filepath)

    container = MainCategoryContainer()#알아서 초기화 하세요
    container.setBox([12, 54, 32, 99])
    container.setLabel(["Top"])  # 상의

    #container(.getDict())를 통해서 json파일을 만들어 내야함

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())
    return path


def testSubCategoryContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath+ip+"subcategory.json"
    path = JsonPath(filepath)

    container = SubCategoryContainer()  # 알아서 초기화 하세요
    container.setLabel(["neat"])

    # container(.getDict())를 통해서 json파일을 만들어 내야함

    print("resource :",resource_path,"| worke space :", filepath)
    print(container.getDict())

    return path
def testPatternContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath+ip+"/pattern.json"
    path = JsonPath(filepath)


    container = SubCategoryContainer()  # 알아서 초기화 하세요
    container.setLabel(["neat"])

    # container(.getDict())를 통해서 json파일을 만들어 내야함

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())

    return path
def testStyleContainer(ip ,resource_path):
    filepath = "./GPU_Worker/"
    filepath = filepath+ip+"/style.json"
    path = JsonPath(filepath)

    container = PatternContainer()  # 알아서 초기화 하세요
    container.setLabel(["solid"])

    # container(.getDict())를 통해서 json파일을 만들어 내야함

    print("resource :", resource_path, "| worke space :", filepath)
    print(container.getDict())

    return path
def ReadJsonFile(json_path):
    with open(json_path, 'r') as f:
        json_data = json.load(f)
    return json_data

id = "Sample"   # user_ip or process id... 등 구별 가능한걸로 설정
fashion = FashionContainer()#생성자는 원하는대로 넣으세요

mc_path = testMainCategoryContainer(id, "main img")
sc_path = testSubCategoryContainer(id, "crop img")
p_path = testPatternContainer(id,"crop img")
s_path = testStyleContainer(id, "main img")


fashion.mc.setDict(json.dump(ReadJsonFile(mc_path))
fashion.sc.setDict(json.dump(ReadJsonFile(sc_path))
fashion.pc.setDict(json.dump(ReadJsonFile(p_path))
fashion.st.setDict(json.dump(ReadJsonFile(s_path))

print(fashion.GetDict())

a="""
if __name__ == '__main__':
    id = "Sample"   # user_ip or process id... 등 구별 가능한걸로 설정    
    fashion = FashionContainer(id, 'C:/fashion')

    fashion.mc.setPath("C:/fashion/main_category")
    fashion.mc.setBox([12, 54, 32, 99])
    fashion.mc.setLabel(["Top"])  # 상의
    fashion.mc.setDict()

    fashion.sc.setPath("C:/sub_category")
    fashion.sc.setLabel(["neat"])
    fashion.sc.setDict()

    fashion.pc.setPath("C:/fashion/pattern")
    fashion.pc.setLabel(["solid"])
    fashion.pc.setDict()

    fashion.st.setPath("C:/fashion/style")
    fashion.st.setLabel(["casual"])
    fashion.st.setDict()

    print(fashion.getDict())
    fashion.setDict()
"""