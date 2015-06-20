<b>disclaimer</b>: the article provides an overview of the specific pattern Repository implementation,considering in detail the methods and features within Incoding Framework. For the best immersion read <a href="http://martinfowler.com/eaaCatalog/repository.html">Repository by Fowler</a>, <a title="Cqrs vs N-layer" href="http://blog.incframework.com/en/cqrs-vs-n-layer/">CQRS vs NLayer</a>

<strong>UPD:</strong> Article source codes are available on <a href="https://github.com/IncodingSoftware/Repository">GitHub</a>
<h3 style="text-align: center;"><span style="font-size: 1.5em;">What do we get ?</span></h3>
Patterns often become not only a tool to struggle against the complexity of the project, but sometimes at a very "dense" use, are themselves a source of problems. So before applying one or another pattern I have been studying its impact on the architecture of the project and the amount of code to implement it.
<ul>
	<li><strong>Abstraction over the database</strong></li>
</ul>
<em>Note: ORM is an abstraction over the database and Repository will be redundant, but eventually I came to the conclusion that it is always better to have a layer between the third-party components, such as ORM, IoC and others.</em>
<ul>
	<li><strong>Uniform interface for providers (Nhibernate, Entity Framework, etc.)</strong></li>
</ul>
<em>Note: The first part partly covers the second point, but it is a different problem. </em>
<h3 style="text-align: center;"><span style="font-size: 1.5em;">And may it be without it?</span></h3>
DataContext on View (linq to sql used the name DataContext for a class of database access), a kind of "meme" in the development of web sites, which is on par with GOD object and other anti-patterns. In fact, a call does not necessarily have to be on View, because when calling DataContext from Controller or Service, the problem remains. What difficulties the use of Data Context without Repository entails:
<ul>
<ul>
	<li><strong>Binding to a specific ORM implementation ( Linq to sql = DataContext, Nhibernate = ISession )</strong></li>
</ul>
</ul>
&nbsp;

<em>Note: on the one hand ORM replacement in the later stages of the project seems to be a controversial and a difficult step, but personal experience convinced (Linq to Sql replacement for Nhibernate), it is sometimes the only way to solve some problems.</em>
<ul>
	<li><strong>There are a lot of low-level methods</strong></li>
</ul>
<em>Note: I hold the opinion that it is much easier to maintain a limited set of methods to work with the database than access to low-level features of a particular ORM operation. </em>
<ul>
	<li><strong>It is difficult to monitor places where the work with a database is going on (Incoding Framework provides access to the Repository only in Query and Command)</strong></li>
</ul>
<em>Note: The main difficulty will be to support UnitOfWork to ensure correct operation of transactions</em>
<ul>
	<li><strong>The difficulties of writing unit tests</strong></li>
</ul>
<em>Note: due to the fact that the code works with 3rd-party objects, then there is difficulty in creating Mock Up</em>

&nbsp;
<h3 style="text-align: center;"><span style="font-size: 1.5em;">CRUD</span></h3>
Repository has methods to perform basic tasks related to the creation (create), reading (read), updating (update) and removing (delete):
<h3>Create, Update, Delete</h3>
<ul>
	<li><strong>Save - </strong>- saves the object to the database</li>
</ul>
<pre class="lang:c# decode:true">Repository.Save(new Product { Code = Code, Name = Name, Price = Price, CreateDt = DateTime.Now });</pre>
<ul>
	<li> <strong>Delete</strong> - deletes the object according to the type and Id</li>
</ul>
<pre class="lang:c# decode:true">Repository.Delete&lt;Product&gt;(Id);</pre>
<ul>
	<li><b>SaveOrUpdate -  </b>- saves or updates the object in the database</li>
</ul>
<pre>Repository.SaveOrUpdate(product);</pre>
<p style="text-align: justify;"><em>Note: SaveOrUpdate method may not be used, because many ORM (Nhibernate, Entity Framework) supports object state tracking (tracking), but if the provider does not have this opportunity, you should always call SaveOrUpdate</em></p>

<pre class="lang:c# decode:true">var product = Repository.GetById&lt;Product&gt;(Id);
product.Name = "New Name"; 
Repository.SaveOrUpdate(product); // if tracking not available</pre>
<h3>Read</h3>
<ul>
	<li><strong>GetById</strong> -returns an object according to the type and Id</li>
</ul>
<pre class="lang:c# decode:true">var product = Repository.GetById&lt;Product&gt;(Id);</pre>
<em>Note: Id parameter is Object, the reason for the absence of a specific type is to maximize flexibility (Id can be string, int, long, guid) of the solution.</em>
<em>Note: If Id null or an object is found, the return will be null</em>
<ul>
	<li><strong>LoadById - </strong>- returns an object according to the type and Id</li>
</ul>
<pre class="lang:c# decode:true">var product = Repository.LoadById&lt;Product&gt;(Id);</pre>
<em>Note: LoadById method works just as well as GetById, with the difference that when calling Load will try to find objects in the Cache, and Get always calls the database</em>
<pre class="lang:c# decode:true">var product = Repository.LoadById&lt;Product&gt;(Id); // get from data base
product = Repository.LoadById&lt;Product&gt;(Id); // get from cache</pre>
<ul>
	<li> <strong>Query</strong> - returns a set of (IQueryable) objects based on the specifications (where, order, fetch, paginated) </li>
</ul>
<pre class="lang:c# decode:true">Repository.Query(whereSpecification: new ProductHavingCodeWhere(Code)
                                            .And(new ProductInPriceWhere(From, To)),
                 orderSpecification: new ProductOrder(OrderBy,Desc))</pre>
<em>Note: The operations manual of the where, order, fetch query specifications are discussed below.</em>
<ul>
	<li><strong>Paginated</strong> - returns paginated result on the basis of specifications (where, order, fetch, paginated) </li>
</ul>
<pre class="lang:c# decode:true">Repository.Paginated(paginatedSpecification: new PaginatedSpecification(1, 10),
                     whereSpecification: new ProductHavingCodeWhere(Code)
                             .And(new ProductInPriceWhere(From, To)),
                     orderSpecification: new ProductOrder(ProductOrder.OrderBy, Desc));</pre>
<em>Note: IncPaginatedResult is an object that was designed to provide a comfortable work with the results that are to be displayed per page. Practice has shown that to construct paged data, you need to know the general (TotalCounts) number of elements, excluding pages and items (Items).</em>
<ul>
	<li><strong>Total Counts</strong>  - the total number of items in the database ( include where )</li>
	<li><strong>Items</strong> - items that are in the range of the current page</li>
</ul>
<em>Note: Paginated Specification algorithm will be discussed below</em>
<h3 style="text-align: center;">Specification</h3>
In C # there is LINQ, which allows to build a query plan, and to implement (broadcast, transmit) them by way of a certain provider. For example, if we chose Nhibernate, as ORM for our application, IQueryable Nhibernate  provider can be used in order to transmit conditions (where, order, fetch, paginated) in SQL.
<pre class="lang:c# decode:true">items.Where(product = &gt; product.Name == "Vlad")</pre>
<strong>What is bad about LINQ?</strong>
Specifications are cover of LINQ expressions into separate classes, which offer the following benefits:
<ul>
	<li><strong>Reuse in different Query</li>
</ul>
<em>Note: a significant advantage, because when writing large projects, it is difficult to maintain "scattered" throughout the code LINQ expressions.</em>
<ul>
	<li><strong>Substitution for mock-up objects in Query tests</li>
</ul>
<em>Note: tests of the specifications will be carried out separately</em>
<ul>
	<li><strong>Encapsulation of additional logic</li>
</ul>

