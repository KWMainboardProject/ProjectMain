<?php
 session_start();
 
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
        <!--<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>-->
        
        <script src="https://code.jquery.com/jquery-latest.js"></script>
        <script>
	        function file_frm_submit(frm) {

		        var fileCheck = frm.upload_file.value;

		        if(!fileCheck) {
			        alert("업로드할 파일을 선택하세요.");
			        return false;
		        }

	        	var formData = new FormData(frm);			// 파일전송을 위한 폼데이터 객체 생성

		        formData.append("message", "ajax로 파일 전송하기");
		        //formData.append("file", jQuery("#upload_file")[0].files[0]);

	        	$.ajax({
		        	url			: 'ajax_file_upload_test.php',
		        	type		: 'POST',
		        	dataType	: 'html',
		        	enctype		: 'multipart/form-data',
		          processData	: false,
		        	contentType	: false,
		        	data		: formData,
		        	async		: false,
		        	success		: function(response) {

                //console.log(response);
              
		        	  }
	          	}).done(function(data){
                //document.writeln(typeof(data));
                alert("완료");
              });
              
	        }
        </script>
        <script>
          var index = 0;
          function semiauto(){
            $.ajax({
            url: "lel_t.php",
            type: "get"
              }).done(function(data) {
            const datat = <?php 
              {$JsonParser = file_get_contents("./json/Top.json");unlink("./json/Top.json");echo $JsonParser;}?>;
            $('#result1_t').text(datat.type[0] + ' : ' + datat.prop[0] );              
            $('#result2_t').text(datat.type[1] + ' : ' + datat.prop[1] );
            $('#result3_t').text(datat.type[2] + ' : ' + datat.prop[2] );              
            $('#result4_t').text(datat.type[3] + ' : ' + datat.prop[3] );
            
            });
            
            $.ajax({
            url: "lel_b.php",
            type: "get"
              }).done(function(data) {
            const datab = <?php 
              {$JsonParser = file_get_contents("./json/Bottom.json");unlink("./json/Bottom.json");echo $JsonParser;}?>;
            $('#result1_b').text(datab.type[0] + ' : ' + datab.prop[0] );              
            $('#result2_b').text(datab.type[1] + ' : ' + datab.prop[1] );
            $('#result3_b').text(datab.type[2] + ' : ' + datab.prop[2] );              
            $('#result4_b').text(datab.type[3] + ' : ' + datab.prop[3] );
            
            });
            
            $.ajax({
            url: "lel_o.php",
            type: "get"
              }).done(function(data) {
            const datao = <?php 
              {$JsonParser = file_get_contents("./json/Outer.json");unlink("./json/Outer.json");echo $JsonParser;}?>;
            $('#result1_o').text(datao.type[0] + ' : ' + datao.prop[0] );              
            $('#result2_o').text(datao.type[1] + ' : ' + datao.prop[1] );
            $('#result3_o').text(datao.type[2] + ' : ' + datao.prop[2] );              
            $('#result4_o').text(datao.type[3] + ' : ' + datao.prop[3] );
            
            });
            $.ajax({
            url: "lel_a.php",
            type: "get"
              }).done(function(data) {
            const dataa = <?php 
              {$JsonParser = file_get_contents("./json/Overall.json");unlink("./json/Overall.json");echo $JsonParser;}?>;
            $('#result1_a').text(dataa.type[0] + ' : ' + dataa.prop[0] );              
            $('#result2_a').text(dataa.type[1] + ' : ' + dataa.prop[1] );
            $('#result3_a').text(dataa.type[2] + ' : ' + dataa.prop[2] );              
            $('#result4_a').text(dataa.type[3] + ' : ' + dataa.prop[3] );
            
            });
            
          }
          
        </script>
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
                <div class="container" >
                  <form name="reqform" method="post"  enctype="multipart/form-data"> 
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
                      <label class="col-md-3 col-form-label" id="testlabel">제품가격</label>
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
                      <div class="col-md-9" >
                        <input class="form-control" type="file" name="upload_file" id="upload_file"  accept="image/png, image/jpg, image/jpeg">
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
                      <button type="button" id="files_send" name = "files_send" value="Upload" class="btn btn-lg btn-outline-primary" onclick=" file_frm_submit(this.form); printResult();semiauto()" >AI Image Analysis (Semi Auto)</button>
                      </div>
                    </div>
                    <div class="mb-3 row">
                      <div id="semi_buttons" class="col-12 d-grid p-1" style="visibility: hidden;">
                      <button type="button" id="topbtn" name = "files_send" value="Upload" class="btn btn-lg btn-outline-primary" onclick="printResult1()" style="visibility: hidden;">Top</button>
                      <button type="button" id="btmbtn" name = "files_send" value="Upload" class="btn btn-lg btn-outline-primary" onclick="printResult2()" style="visibility: hidden;">Bottom</button>
                      <button type="button" id="outbtn" name = "files_send" value="Upload" class="btn btn-lg btn-outline-primary" onclick="printResult3()" style="visibility: hidden;">Outer</button>
                      <button type="button" id="overbtn" name = "files_send" value="Upload" class="btn btn-lg btn-outline-primary" onclick="printResult4()" style="visibility: hidden;">Overall</button>
                      </div>
                    </div>
                    <!--AI 세미오토 결과 받아왔을 때 or 세미오토 버튼이 눌렸을 때 => visibility 속성 변경(hidden -> visible)-->
                    <div id="semi_result" class="mb-3 row" style="visibility: hidden;">
                      <label class="col-md-3 col-form-label">AI 분석 결과</label>
                      <div class="col-md-9">
                        <!--아래 항목들 결과로 받아온 것대로 기입-->
                        <p name="preMainCat" id="result1_t">Main Category: ..</p>
                        <p name="preSubCat" id="result2_t">Sub Category: ..</p>
                        <p name="preStyle" id="result3_t">Style: ..</p>
                        <p name="prePattern" id="result4_t">Pattern: ..</p>
                        <p>
                          Main Color: 
                          <canvas name="preColor" style="width: 30px; height: 30px; background-color: rgb(40,42,57);">
                        </p>
                      </div>
                    </div>
                    <div id="semi_result2" class="mb-3 row" style="visibility: hidden;">
                      <label class="col-md-3 col-form-label">AI 분석 결과</label>
                      <div class="col-md-9">
                        <!--아래 항목들 결과로 받아온 것대로 기입-->
                        <p name="preMainCat" id="result1_b">Main Category: ..</p>
                        <p name="preSubCat" id="result2_b">Sub Category: ..</p>
                        <p name="preStyle" id="result3_b">Style: ..</p>
                        <p name="prePattern" id="result4_b">Pattern: ..</p>
                        <p>
                          Main Color: 
                          <canvas name="preColor" style="width: 30px; height: 30px; background-color: rgb(40,42,57);">
                        </p>
                      </div> 
                    </div>
                    <div id="semi_result3" class="mb-3 row" style="visibility: hidden;">
                      <label class="col-md-3 col-form-label">AI 분석 결과</label>
                      <div class="col-md-9">
                        <!--아래 항목들 결과로 받아온 것대로 기입-->
                        <p name="preMainCat" id="result1_o">Main Category: ..</p>
                        <p name="preSubCat" id="result2_o">Sub Category: ..</p>
                        <p name="preStyle" id="result3_o">Style: ..</p>
                        <p name="prePattern" id="result4_o">Pattern: ..</p>
                        <p>
                          Main Color: 
                          <canvas name="preColor" style="width: 30px; height: 30px; background-color: rgb(40,42,57);">
                        </p>
                    </div>
                    <div id="semi_result4" class="mb-3 row" style="visibility: hidden;">
                      <label class="col-md-3 col-form-label">AI 분석 결과</label>
                      <div class="col-md-9">
                        <!--아래 항목들 결과로 받아온 것대로 기입-->
                        <p name="preMainCat" id="result1_a">Main Category: ..</p>
                        <p name="preSubCat" id="result2_a">Sub Category: ..</p>
                        <p name="preStyle" id="result3_a">Style: ..</p>
                        <p name="prePattern" id="result4_a">Pattern: ..</p>
                        <p>
                          Main Color: 
                          <canvas name="preColor" style="width: 30px; height: 30px; background-color: rgb(40,42,57);"></div>
                        </p>
                    </div>
                  </form>
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
                    <input class="form-control" type="file" accept="image/png, image/jpg" multiple>
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
              var result = document.getElementById('semi_buttons');
              result.style.visibility = "visible"
              <?php  
                if(file_exists("./json/Top.json")){
                  echo "var result = document.getElementById('topbtn');
                  result.style.visibility = 'visible'";}
                if(file_exists("./json/Bottom.json")){
                  echo "var result = document.getElementById('btmbtn');
                  result.style.visibility = 'visible'";}
                if(file_exists("./json/Outer.json")){
                  echo "var result = document.getElementById('outbtn');
                  result.style.visibility = 'visible'";}
                if(file_exists("./json/Overall.json")){
                  echo "var result = document.getElementById('overbtn');
                  result.style.visibility = 'visible'";}   ?>
            }
            function printResult1() {
              var result = document.getElementById('semi_result');
              result.style.visibility = "visible"
              var result = document.getElementById('semi_result2');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result3');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result4');
              result.style.visibility = "hidden"
            }
            function printResult2() {
              var result = document.getElementById('semi_result');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result2');
              result.style.visibility = "visible"
              var result = document.getElementById('semi_result3');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result4');
              result.style.visibility = "hidden"
            }
            function printResult3() {
              var result = document.getElementById('semi_result');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result2');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result3');
              result.style.visibility = "visible"
              var result = document.getElementById('semi_result4');
              result.style.visibility = "hidden"
            }
            function printResult4() {
              var result = document.getElementById('semi_result');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result2');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result3');
              result.style.visibility = "hidden"
              var result = document.getElementById('semi_result4');
              result.style.visibility = "visible"
            }
        </script>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
        <script> function test(){ $.ajax({url:"product-regist.php" }) } </script> 
    </body>
</html>
