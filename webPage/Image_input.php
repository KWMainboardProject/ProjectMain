<?php session_start(); ?>
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Image Input Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />

        <script src="example.js"></script>
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

		      		console.log(response);
              

		        	  }
	          	}).done(function(data){
                alert(data);
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
                    <?php if(!is_null($_SESSION['id'])){
                        $idtext = $_SESSION['id']."님으로 로그인 중입니다.";
                    }else{
                        $idtext = "로그인을 해주세요.";
                    } ?>
                    <a class="text"> <?php echo $idtext;?></a>}
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="index.php">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="about.php">About</a></li>
                            <?php if(!is_null($_SESSION['id'])){
                                $idbtn = "Logout";
                                $_SESSION['islogin'] = false;
                            }else{
                                $idbtn = "Login";
                                $_SESSION['islogin'] = false;
                                
                            } ?>
                            <li class="nav-item"><a class="nav-link" href="login.php"><?php echo $idbtn;?></a></li>
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
                    <h2 class="mb-5" style="font-weight: bolder;">Similar Image Search</h2>
                    <hr class="mb-5">
                    <div class="mt-5 mb-3 row">
                        <div class="col-lg-6">
                            <form name="reqform" method="post"  enctype="multipart/form-data">
                            <input type="file" onchange="readURL(this);" name='upload_file' id='upload_file' accept="image/png, image/jpg, image/jpeg">
                            <div class="alert alert-secondary mb-3" role="alert">
                                <ul>
                                  <li>파일 사이즈: 1MB 이하</li>
                                  <li>파일 확장자: jpeg, png, jpg만 가능</li>
                                </ul>
                            </div>
                            <div class="mb-3 row">
                                <div class="col-6 d-grid p-1 justify-content-center">
                                    <!--search 버튼 이미지 유무에 따른 비활성화-->
                                    <button type="button" disabled class="btn btn-lg btn-outline-dark" id="Search_btn" data-bs-toggle="modal" data-bs-target="#CF_Modal">
                                        Search
                                    </button>
                                </div>
                                <!-- Confirm(확인) Modal Section -->
                                <div class="modal fade" id="CF_Modal" tabindex="-1" aria-labelledby="CF_ModalLabel" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="object-fit: contain;">
                                                <h5 class="modal-title" id="CF_ModalLabel">Confirm!</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body" style="object-fit: contain;">
                                                Is this the picture you are looking for?
                                            </div>
                                            <div class="modal-footer">
                                                <a href="Image_result.php">
                                                    <button type="button" class="btn btn-primary" onclick="file_frm_submit(this.form)">Yes</button>
                                                </a>
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <a href="index.php" class="col-6 d-grid p-1 justify-content-center">
                                    <button type="button" class="btn btn-lg btn-outline-danger" id="Cancel_btn">Cancel</button>
                                </a>
                            </div>
                            </form>
                        </div>
                        <div class="col-lg-6">
                            <p style="font-weight:bold">이미지 미리보기</p>
                            <img class="ImgPreview" id="preview" style="max-width: 100%; height: auto; object-fit: contain;">
                        </div>
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
        <!--Additional JS Section-->
        <script>
            function readURL(input) {
                if(input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function(e) {
                        document.getElementById('preview').src = e.target.result;
                    };
                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    document.getElementById('preview').src = "";
                }

                document.getElementById('Search_btn').disabled = false;
            }
        </script>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
    </body>
</html>