from class_file import FashionContainer

if __name__ == '__main__':
    id = "Sample"   # user_ip or process id... 등 구별 가능한걸로 설정
    fashion = FashionContainer(id, 'C:/fashion')

    fashion.mc.setPath("C:/fashion/main_category")
    fashion.mc.setBox([12, 54, 32, 99])
    fashion.mc.setLabel(["U"])  # 상의
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

    fashion.getDict()
    fashion.setDict()
