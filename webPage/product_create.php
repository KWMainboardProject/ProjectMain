<?php 

function php_func(){
  
require_once 'config/ftp_connect.php';


// FTP서버 접속
$conn_id = ftp_connect($ftp_server,$ftp_port);

// FTP서버 접속 실패한 경우
if($conn_id == false){
    print_r("[IP:".$ftp_server.":".$ftp_port ."] FTP 서버 접속 실패");
}
echo "hello";

// FTP서버 로그인
$login_result = ftp_login($conn_id, $ftp_user_name, $ftp_user_pass);

// 로그인 실패한 경우
if($login_result == false){
    print_r("[IP:".$ftp_server.":".$ftp_port ."], [USER:".$ftp_user_name."] 로그인 실패");
}

// 패시브 모드 설정
ftp_pasv($conn_id, true);

// FTP 서버에 파일 전송
if (ftp_put($conn_id, $ftp_remote_file, $ftp_send_file, FTP_BINARY)) {
    print_r("파일 전송 (ftp) -> UPLOAD 성공");
} else {
    print_r("파일 전송 (ftp:".$conn_id.",".$ftp_remote_file.",".$ftp_send_file.") -> UPLOAD 실패");
}

// FTP 서버와 연결 끊음
ftp_close($conn_id);

}
?>
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Product Register Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />

        <script src="example.js"></script>
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    </head>
    <body class="d-flex flex-column h-100">
        <main class="flex-shrink-0">
            <!-- Navigation-->
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
              <div class="container px-5">
                  <a class="navbar-brand" href="index.php">MainBoard</a>
                  <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                  <div class="collapse navbar-collapse" id="navbarSupportedContent">
                      <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                          <li class="nav-item"><a class="nav-link" href="index.php">Home</a></li>
                          <li class="nav-item"><a class="nav-link" href="about.php">About</a></li>
                          <li class="nav-item"><a class="nav-link" href="login.php">Login</a></li>
                          <li class="nav-item dropdown">
                              <a class="nav-link dropdown-toggle" id="navbarDropdownBlog" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Collections</a>
                              <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownBlog">
                                  <li><a class="dropdown-item" href="Outer-list.php">Outer</a></li>
                                  <li><a class="dropdown-item" href="Top-list.php">Top</a></li>
                                  <li><a class="dropdown-item" href="Bottom-list.php">Bottom</a></li>
                                  <li><a class="dropdown-item" href="Overall-list.php">Overall</a></li>
                              </ul>
                          </li>
                          <li class="nav-item"><a class="nav-link" href="product_create.php">Product Register</a></li>
                          <li name="nav_myproducts" class="nav-item"><a class="nav-link" href="myProduct-list.php" style="display: none;">My Products</a></li>
                      </ul>
                  </div>
              </div>
          </nav>
            <!-- Page Content-->
            <section class="py-5">
                <div class="container">
                    <h2 class="text-center fw-bolder mb-5">Product Register</h2>
                    <hr>
                    <!--제품 이름 기입-->
                    <div class="mb-3 row">
                      <label class="col-md-3 col-form-label">제품명</label>
                      <div class="col-md-9">
                        <input type="text" id="productid" name="productid" class="form-control">
                      </div>
                    </div>
                    <!--제품 가격 기입-->
                    <div class="mb-3 row">
                      <label class="col-md-3 col-form-label">제품가격</label>
                      <div class="col-md-9">
                        <div class="input-group mb-3">
                          <input type="number" class="form-control">
                          <span class="input-group-text">원</span>
                        </div>
                      </div>
                    </div>
                    <!--제품 설명 기입-->
                    <div class="mb-3 row">
                        <label class="col-md-3 col-form-label">제품설명</label>
                        <div class="col-md-9">
                          <input type="text" class="form-control">
                        </div>
                      </div>
                    <!--제품 썸네일 기입-->
                    <div class="mb-3 row">
                      <label class="col-md-3 col-form-label">썸네일 이미지(AI분석)</label>
                      <div class="col-md-9">
                        <input class="form-control" type="file"  accept="image/png, image/jpg, image/jpeg">
                        <div class="alert alert-secondary" role="alert">
                          <ul>
                            <li>이미지 사이즈: 300*400</li>
                            <li>파일 사이즈: 1MB 이하</li>
                            <li>파일 확장자: jpeg, png, jpg만 가능</li>
                          </ul>
                        </div>
                      </div>
                    </div>
                    <!--AI 세미오토 버튼-->
                    <div class="mb-3 row">
                      <div class="col-12 d-grid p-1">
                        <button type="button" class="btn btn-lg btn-outline-primary" onclick="printResult(); test()">AI Image Analysis (Semi Auto)</button>
                      </div>
                    </div>
                    <!--AI 세미오토 결과 받아왔을 때 or 세미오토 버튼이 눌렸을 때 => visibility 속성 변경(hidden -> visible)-->
                    <div id="semi_result" class="mb-3 row" style="visibility: hidden;">
                      <label class="col-md-3 col-form-label">AI 분석 결과</label>
                      <div class="col-md-9">
                        <!--아래 항목들 결과로 받아온 것대로 기입-->
                        <p name="preMainCat">Main Category: ..</p>
                        <p name="preSubCat">Sub Category: ..</p>
                        <p name="preStyle">Style: ..</p>
                        <p name="prePattern">Pattern: ..</p>
                        <p>
                          Main Color: 
                          <canvas name="preColor" style="width: 30px; height: 30px; background-color: rgb(255,166,0);"></div>
                        </p>
                      </div>
                    </div>
                </div>
                <div class="container">
                  <!--제품 카테고리 기입-->
                  <div class="mb-3 row">
                    <label class="col-md-3 col-form-label">제품 카테고리</label>
                    <div class="col-md-9">
                      <div class="row">
                        <div class="col-auto">
                          <select class="form-select" onchange="categoryChange(this)">
                            <option>Main Category</option>
                            <option value="a">Outer</option>
                            <option value="b">Top</option>
                            <option value="c">Bottom</option>
                            <option value="d">Overall</option>
                          </select>
                        </div>
                        <div class="col-auto">
                          <select class="form-select" id="good">
                            <option>Sub Category</option>
                          </select>
                        </div>
                      </div>
                    </div>
                  </div>
                  <!--제품 상세 이미지 기입-->
                  <div class="mb-3 row">
                  <label class="col-md-3 col-form-label">제품 이미지</label>
                  <div class="col-md-9">
                    <input class="form-control" type="file" accept="image/png, image/jpg, image/jpeg" multiple>
                    <div class="alert alert-secondary" role="alert">
                      <ul>
                        <li>최대 5개 가능</li>
                        <li>이미지 사이즈: 600*800</li>
                        <li>파일 사이즈: 1MB 이하</li>
                        <li>파일 확장자: jpeg, png, jpg만 가능</li>
                      </ul>
                    </div>
                  </div>
                  </div>
                  <!--등록 / 취소 버튼-->
                  <div class="mb-3 row">
                    <div class="col-6 d-grid p-1">
                      <!--제품 등록 버튼 => 여기에서 DB에 insert 필요-->
                        <button type="button" class="btn btn-lg btn-outline-dark" id="Register_btn" data-bs-toggle="modal" data-bs-target="#RC_Modal">
                          Register
                        </button>
                    </div>
                    <!-- RC(등록 완료) Modal Section -->
                    <div class="modal fade" id="RC_Modal" data-bs-backdrop="static" tabindex="-1" aria-labelledby="RC_ModalLabel" aria-hidden="true">
                      <div class="modal-dialog">
                          <div class="modal-content">
                              <div class="modal-header" style="object-fit: contain;">
                                  <h5 class="modal-title" id="RC_ModalLabel">Register Complete</h5>
                              </div>
                              <div class="modal-body" style="object-fit: contain;">
                                  Your Product is successfully registered in our store. Thank you.
                              </div>
                              <div class="modal-footer">
                                  <a href="index.php">
                                      <button type="button" class="btn btn-primary">OK</button>
                                  </a>
                              </div>
                          </div>
                      </div>
                  </div>
                    <a href="index.php" class="col-6 d-grid p-1">
                        <button type="button" class="btn btn-lg btn-outline-danger">Cancel</button>
                    </a>
                  </div>
                </div>
            </section>
        </main>
        <!-- Footer-->
        <footer class="bg-dark py-4 mt-auto">
            <div class="container px-5">
                <div class="row align-items-center justify-content-between flex-column flex-sm-row">
                    <div class="col-auto"><div class="small m-0 text-white">Copyright &copy; MainBoard 2022</div></div>
                    <div class="col-auto">
                        <a class="link-light small" href="#!">Privacy</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Terms</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Contact</a>
                    </div>
                </div>
            </div>
        </footer>
        <script>
          function categoryChange(e) {
                var good_a = ["Short/Hoodie", "Short/NormarlCollar", "Short/StandCollar", "Short/Blouson", "Short/NoneCollar", "Blazer", "Vest", "Long/Hoodie", "Long/None"];
                var good_b = ["Pullover", "Hoodie", "Long Sleeve tee", "Short Sleeve tee", "Long Sleeve shirts", "Short Sleeve shirts", "Turtleneck", "Sleeveless"];
                var good_c = ["LongPants/Jogger", "LongPants/Normal", "ShortPants", "LongSkirt", "ShortSkirt"];
                var good_d = ["Jumpsuit", "Onepiece"];
                var target = document.getElementById("good");

                if(e.value == "a") var d = good_a;
                else if(e.value == "b") var d = good_b;
                else if(e.value == "c") var d = good_c;
                else if(e.value == "d") var d = good_d;

                target.options.length = 0;

                for (x in d) {
                    var opt = document.createElement("option");
                    opt.value = d[x];
                    opt.innerHTML = d[x];
                    target.appendChild(opt);
                }	
            }

            function printResult() {
              var result = document.getElementById('semi_result');
              result.style.visibility = "visible"
            }
        </script>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
        <script>
          function clickMe(){
            var result ="<?php php_func();?>"
            document.write(result);
          }
        </script>
        <script> function test(){ $.ajax({url:"product-regist.php", success:function(result){ $("div").text(result);} }) } </script>
    </body>
</html>
