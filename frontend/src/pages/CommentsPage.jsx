import PageLayout from '../Components/PageLayout';
import './CommentsPage.css';

function CommentsPage() {
  return (
    <PageLayout title="Comments">
      <div className="comments-page__card">
        <h2>Visitor Comments</h2>
        <p>
          Comments are currently shown inside each artifact details page so users can read feedback
          in the context of the artwork.
        </p>
        <p>
          Open the Artifacts page, choose an artifact, and view its full comments section there.
        </p>
      </div>
    </PageLayout>
  );
}

export default CommentsPage;
