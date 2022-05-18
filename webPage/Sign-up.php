<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>Modern Business - Start Bootstrap Template</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />
        <link href="css/styleOwn.css" rel="stylesheet">
    </head>
    <body class="d-flex flex-column">
        <main class="flex-shrink-0">
            <!-- Navigation-->
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <div class="container px-5">
                    <a class="navbar-brand" href="index.html">MainBoard</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="index.html">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="about.html">About</a></li>
                            <li class="nav-item"><a class="nav-link" href="login.html">Login</a></li>
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" id="navbarDropdownBlog" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Collections</a>
                                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownBlog">
                                    <li><a class="dropdown-item" href="product-list.html">Outer</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Top</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Bottom</a></li>
                                    <li><a class="dropdown-item" href="product-list.html">Overall</a></li>
                                </ul>
                            </li>
                            <li class="nav-item"><a class="nav-link" href="product_create.html">Product Register</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Page Content-->
            <section class="py-5">
                <div class="container px-5 my-5">
                    <div class="row gx-5">
                        <div class="col-lg-7">
                            <!-- Post content-->
                            <section class="login-form">
                                <h1>회원 가입 폼</h1>
                                <form action="register.php" method="post" name="regiform" id="regist_form" class="form" onsubmit="return sendit()">
                                    <div class="int-area">
                                        <input type="text" name="userid" id="userid" autocomplete="off" required>
                                        <label for="id">USER ID</label>
                                        <button type="button"  value="check" onclick="checkId()">CHECK</button>
                                        <p id="result">&nbsp;</p>
                                    </div>
                                    <div class="int-area">
                                        <input type="password" name="userpw" id="userpw" autocomplete="off" required>
                                        <label for="userpw">PASSWORD</label>
                                    </div>
                                    <div class="int-area">
                                        <input type="password" name="userpw_ch" id="userpw_ch" autocomplete="off" required>
                                        <label for="userpw_ch">PASSWORD Repeat</label>
                                    </div>
                                    <div class style="padding: 40px 0px;">
                                        <div class="select" style="float:left;">
                                            <label >Gender</label>    
                                            <select name="gender" id="gender">
                                                <option selected disabled>Gender</option>
                                                <option value="Man">Man</option>
                                                <option value="Woman">Woman</option>
                                            </select>
                                        </div>
                                        <p class="hobbystr">
                                            <label for="drive">Drive <input type="checkbox" name="hobby[]" id="drive" value="Drive"></label>
                                            <label for="movie">Movie <input type="checkbox" name="hobby[]" id="movie" value="Movie"></label>
                                            <label for="study">Study <input type="checkbox" name="hobby[]" id="study" value="Study"></label>
                                            <label for="game">Game <input type="checkbox" name="hobby[]" id="game" value="Game"></label> 
                                            <label for="health">Health <input type="checkbox" name="hobby[]" id="health" value="Health"></label>
                                            <label for="coding">Coding <input type="checkbox" name="hobby[]" id="coding" value="Coding"></label>
                                        </p>
                                    </div>
                                    <div class="btn-area">
                                        <button type ="submit">회원 가입</button>
                                        
                                    </div>
                                </form>
                            </section>
                        </div>
                    </div>
                </div>
            </section>
        </main>
        <!-- Footer-->
        <footer class="bg-dark py-4 mt-auto">
            <div class="container px-5">
                <div class="row align-items-center justify-content-between flex-column flex-sm-row">
                    <div class="col-auto"><div class="small m-0 text-white">Copyright &copy; Your Website 2022</div></div>
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
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
        <script src="js/resist.js"></script>
    </body>
</html>
