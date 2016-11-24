//LIFECYCLE METHODS

//  componentWillMount – Invoked once, on both client & server before rendering occurs.
//  componentDidMount – Invoked once, only on the client, after rendering occurs.
//  shouldComponentUpdate – Return value determines whether component should update.
//  componentWillUnmount – Invoked prior to unmounting component.

//SPECS

//getInitialState – Return value is the initial value for state.
//getDefaultProps – Sets fallback props values if props aren’t supplied.
//mixins – An array of objects, used to extend the current component’s functionality.

//-CommentBox
//  - CommentList
//    - Comment
//    - Comment
//    - Comment
//  - CommentForm

// sapmle data
//var data = [
//  { Author: "Daniel Lo Nigro", Text: "Hello ReactJS.NET World!" },
//  { Author: "Pete Hunt", Text: "This is one comment" },
//  { Author: "Jordan Walke", Text: "This is *another* comment" }
//];

//<CommentList data={this.props.data} />
// this.props are immutable. They are passed from the parent and are "owned" by the parent.
// this.state is private to the component and can be changed by calling this.setState()

var CommentBox = React.createClass({
  loadCommentsFromServer: function() {
    var xhr = new XMLHttpRequest();
    xhr.open('get', this.props.url, true);
    xhr.onload = function() {
      var data = JSON.parse(xhr.responseText);
      this.setState({ data: data });
    }.bind(this);
    xhr.send();
  },
  handleCommentSubmit: function(comment) {
    var comments = this.state.data;
    var newComments = comments.concat([comment]);
    this.setState({data: newComments});

    var data = new FormData();
    data.append('Author', comment.Author);
    data.append('Text', comment.Text);

    var xhr = new XMLHttpRequest();
    xhr.open('post', this.props.submitUrl, true);
    xhr.onload = function() {
      this.loadCommentsFromServer();
    }.bind(this);
    xhr.send(data);
  },
  getInitialState: function() {
      //return { data: [] };
      return { data: this.props.initialData };
  },
  componentDidMount: function () {
    //this.loadCommentsFromServer();
    window.setInterval(this.loadCommentsFromServer, this.props.pollInterval);
  },
  render: function() {
    return (
      <div className="commentBox">
        <h1>Comments</h1>
        <CommentList data={this.state.data} />
        <CommentForm onCommentSubmit={this.handleCommentSubmit} />
      </div>
    );
  }
});

var CommentList = React.createClass({
  render: function() {
    var commentNodes = this.props.data.map(function(comment) {
      return (
        <Comment author={comment.Author} key={comment.Id}>{comment.Text}
        </Comment>
      );
    });
    return (
      <div className="commentList">{commentNodes}
      </div>
    );
  }
});

var CommentForm = React.createClass({
  getInitialState: function() {
    return {author: '', text: ''};
  },
  handleAuthorChange: function(e) {
    this.setState({author: e.target.value});
  },
  handleTextChange: function(e) {
    this.setState({text: e.target.value});
  },
  handleSubmit: function(e) {
    e.preventDefault();
    var author = this.state.author.trim();
    var text = this.state.text.trim();
    if (!text || !author) {
      return;
    }
    this.props.onCommentSubmit({Author: author, Text: text});
    this.setState({author: '', text: ''});
  },
  render: function() {
    return (
      <form className="commentForm" onSubmit={this.handleSubmit}>
        <input type="text"
               placeholder="Your name"
               value={this.state.author}
               onChange={this.handleAuthorChange} />
        <input type="text"
               placeholder="Say something..."
               value={this.state.text}
               onChange={this.handleTextChange} />
        <input type="submit" value="Post" />
      </form>
    );
  }
});

var Comment = React.createClass({
    render: function () {
        var converter = new Showdown.converter();
        var rawMarkup = converter.makeHtml(this.props.children.toString());
        return (
          <div className="comment">
            <h2 className="commentAuthor">{this.props.author}</h2>{this.props.children}
              <span dangerouslySetInnerHTML={{__html: rawMarkup}} />
          </div>
      );
    }
});

//Client-side rendering
//ReactDOM.render(
//    <CommentBox url="/comments" submitUrl="/comments/new" pollInterval={2000} />,document.getElementById('content')
//);

////////////////////////////////////////////////////////////////////////////

var Counter = React.createClass({
    incrementCount: function(){
        this.setState({
            count: this.state.count + 1
        });
    },
    getInitialState: function(){
        return {
            count: 0
        }
    },
    render: function(){
        return (
          <div class="my-component">
            <h1>Count: {this.state.count}</h1>
            <button type="button" onClick={this.incrementCount}>Increment</button>
          </div>
      );
    }
});

//ReactDOM.render(<Counter/>, document.getElementById('mount-point'));